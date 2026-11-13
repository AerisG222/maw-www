import { createEffect, onCleanup, onMount, type Component } from "solid-js";

export type MapWidgetProps = {
    poi: google.maps.LatLng;
    center: google.maps.LatLng;
    zoom: number;
    colorScheme: google.maps.ColorScheme;
    otherMapBounds?: google.maps.LatLngBounds;
    centerChanged: (center: google.maps.LatLng) => void;
    boundsChanged: (bounds: google.maps.LatLngBounds) => void;
    zoomChanged: (zoom: number) => void;
};

const MapWidget: Component<MapWidgetProps> = (props) => {
    let map: google.maps.Map | undefined;
    let mapEl!: HTMLDivElement;
    let poiMarker: google.maps.marker.AdvancedMarkerElement | null = null;
    let boundsRectangle: google.maps.Rectangle | null = null;

    const clearBoundsRectangle = () => {
        if (boundsRectangle) {
            boundsRectangle.setMap(null);
            boundsRectangle = null;
        }
    };

    const updateBounds = () => {
        const thisMapBounds = map?.getBounds();

        if (!thisMapBounds || !props.otherMapBounds) {
            return;
        }

        const weAreLarger = thisMapBounds.getSouthWest().lat() < props.otherMapBounds.getSouthWest().lat();

        if (!weAreLarger) {
            clearBoundsRectangle();

            return;
        }

        if (!boundsRectangle) {
            boundsRectangle = new google.maps.Rectangle({
                fillColor: "#c00",
                strokeColor: "#c00",
                clickable: false,
                map: map
            });
        }

        boundsRectangle.setBounds(props.otherMapBounds);
    };

    onMount(() => {
        // zoom and colorScheme are read once: the map only takes its color scheme when it is
        // made, so the parent makes a new map for a new theme, starting at the last zoom
        map = new google.maps.Map(mapEl, {
            mapId: "db13bbf18252cc83",
            zoom: props.zoom,
            colorScheme: props.colorScheme,
        });

        const boundsListener = map.addListener("bounds_changed", () => {
            const b = map?.getBounds();

            if (b) {
                updateBounds();
                props.boundsChanged(b);
            }
        });

        const centerListener = map.addListener("center_changed", () => {
            const c = map?.getCenter();

            if (c) {
                props.centerChanged(c);
            }
        });

        const zoomListener = map.addListener("zoom_changed", () => {
            const z = map?.getZoom();

            if (z !== undefined) {
                props.zoomChanged(z);
            }
        });

        onCleanup(() => {
            boundsListener.remove();
            centerListener.remove();
            zoomListener.remove();
            clearBoundsRectangle();
            poiMarker?.remove();
            poiMarker = null;
            map = undefined;
        });
    });

    createEffect(() => {
        const poi = props.poi;

        if (map && poi) {
            poiMarker?.remove();

            poiMarker = new google.maps.marker.AdvancedMarkerElement({
                map: map,
                position: poi,
            });
        }
    });

    createEffect(() => {
        if (map && props.center) {
            map.setCenter(props.center);
        }
    });

    createEffect(() => {
        if (map && props.otherMapBounds) {
            updateBounds();
        }
    });

    return <div class="maw:mb-4 maw:w-full maw:h-[480px]" ref={mapEl}></div>;
};

export default MapWidget;
