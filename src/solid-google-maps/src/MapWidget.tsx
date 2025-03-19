import { Component, createEffect, onMount } from "solid-js";

export type MapWidgetProps = {
    mapApi: google.maps.MapsLibrary;
    markerApi: google.maps.MarkerLibrary;
    poi: google.maps.LatLng;
    center: google.maps.LatLng;
    zoom: number;
    otherMapBounds?: google.maps.LatLngBounds;
    centerChanged: (center: google.maps.LatLng) => void;
    boundsChanged: (bounds: google.maps.LatLngBounds) => void;
};

const MapWidget: Component<MapWidgetProps> = (props) => {
    let map: google.maps.Map;
    let mapEl!: HTMLDivElement;
    var poiMarker: google.maps.marker.AdvancedMarkerElement | null = null;
    var boundsRectangle: google.maps.Rectangle | null = null;

    const updateBounds = () => {
        const thisMapBounds = map!.getBounds();

        if (thisMapBounds && props.otherMapBounds) {
            const weAreLarger = thisMapBounds.getSouthWest().lat() < props.otherMapBounds.getSouthWest().lat();

            if (weAreLarger) {
                if (!boundsRectangle) {
                    boundsRectangle = new props.mapApi.Rectangle({
                        fillColor: "#c00",
                        strokeColor: "#c00",
                        clickable: false,
                        map: map
                    });
                }

                boundsRectangle.setBounds(props.otherMapBounds);
            } else {
                if (boundsRectangle) {
                    boundsRectangle.setMap(null);
                    boundsRectangle = null;
                }
            }
        }
    };

    onMount(() => {
        map = new props.mapApi.Map(mapEl, {
            mapId: "db13bbf18252cc83",
            zoom: props.zoom,
        });

        map.addListener("bounds_changed", () => {
            const b = map.getBounds();

            if (b) {
                updateBounds();
                props.boundsChanged(b);
            }
        });

        map.addListener("center_changed", () => {
            const c = map.getCenter();

            if (c) {
                props.centerChanged(c);
            }
        });
    });

    createEffect(() => {
        if (map && props.poi) {
            if (poiMarker) {
                poiMarker.remove();
                poiMarker = null;
            }

            poiMarker = new props.markerApi.AdvancedMarkerElement({
                map: map,
                position: props.poi,
            });
        }
    });

    createEffect(() => {
        if (map && props.center) {
            map!.setCenter(props.center);
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
