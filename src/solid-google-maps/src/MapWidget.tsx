import { createEffect, onCleanup, onMount, type Component } from "solid-js";

export type MapWidgetProps = {
    poi: google.maps.LatLng;
    center: google.maps.LatLng;
    zoom: number;
    otherMapBounds?: google.maps.LatLngBounds;
    centerChanged: (center: google.maps.LatLng) => void;
    boundsChanged: (bounds: google.maps.LatLngBounds) => void;
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
        map = new google.maps.Map(mapEl, {
            mapId: "db13bbf18252cc83",
            zoom: props.zoom,
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

        onCleanup(() => {
            boundsListener.remove();
            centerListener.remove();
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
