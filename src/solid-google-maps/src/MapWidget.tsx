import { createEffect, createSignal, onSettled, untrack, type Component } from "solid-js";

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
    let mapEl!: HTMLDivElement;
    let boundsRectangle: google.maps.Rectangle | null = null;
    // set while the center is being pushed in from the parent, so the move is not reported
    // back as if the user made it
    let applyingCenter = false;

    // the map only exists once the element is on the page.  holding it in a signal lets
    // the effects below wait for it, rather than depend on running after onSettled
    const [map, setMap] = createSignal<google.maps.Map>();
    // this map's own bounds, so the outline below follows both maps reactively
    const [bounds, setBounds] = createSignal<google.maps.LatLngBounds>();

    const clearBoundsRectangle = () => {
        if (boundsRectangle) {
            boundsRectangle.setMap(null);
            boundsRectangle = null;
        }
    };

    // outlines the other map's area on this one, when this one shows more
    const updateBounds = (m: google.maps.Map, thisMapBounds: google.maps.LatLngBounds, otherMapBounds: google.maps.LatLngBounds) => {

        const weAreLarger = thisMapBounds.getSouthWest().lat() < otherMapBounds.getSouthWest().lat();

        if (!weAreLarger) {
            clearBoundsRectangle();

            return;
        }

        if (!boundsRectangle) {
            boundsRectangle = new google.maps.Rectangle({
                fillColor: "#c00",
                strokeColor: "#c00",
                clickable: false,
                map: m
            });
        }

        boundsRectangle.setBounds(otherMapBounds);
    };

    onSettled(() => {
        // zoom and colorScheme are read once, on purpose: the map only takes its color scheme
        // when it is made, so the parent makes a new map for a new theme, starting at the
        // last zoom
        const m = new google.maps.Map(mapEl, {
            mapId: "db13bbf18252cc83",
            zoom: untrack(() => props.zoom),
            colorScheme: untrack(() => props.colorScheme),
        });

        const listeners = [
            m.addListener("bounds_changed", () => {
                const b = m.getBounds();

                if (b) {
                    setBounds(b);
                    props.boundsChanged(b);
                }
            }),

            m.addListener("center_changed", () => {
                const c = m.getCenter();

                if (c && !applyingCenter) {
                    props.centerChanged(c);
                }
            }),

            m.addListener("zoom_changed", () => {
                const z = m.getZoom();

                if (z !== undefined) {
                    props.zoomChanged(z);
                }
            }),
        ];

        setMap(m);

        return () => {
            listeners.forEach(l => l.remove());
            clearBoundsRectangle();
        };
    });

    createEffect(
        () => ({ m: map(), poi: props.poi }),
        ({ m, poi }) => {
            if (!m || !poi) {
                return;
            }

            const poiMarker = new google.maps.marker.AdvancedMarkerElement({ map: m, position: poi });

            return () => poiMarker.remove();
        }
    );

    /*
       The center is shared: a drag on either map sets it in the parent, which hands it to
       both.  The map that was dragged is already there, so it is left alone - moving it
       would fight the drag.  The other one is moved without reporting it: the map snaps the
       center to what it can show at its zoom, so what it would report differs a hair from
       what it was given, and passing that back on starts the two maps relaying the center
       between them without end.
    */
    createEffect(
        () => ({ m: map(), center: props.center }),
        ({ m, center }) => {
            if (!m || !center || m.getCenter()?.equals(center)) {
                return;
            }

            applyingCenter = true;

            try {
                m.setCenter(center);
            } finally {
                applyingCenter = false;
            }
        }
    );

    createEffect(
        () => ({ m: map(), thisMapBounds: bounds(), otherMapBounds: props.otherMapBounds }),
        ({ m, thisMapBounds, otherMapBounds }) => {
            if (m && thisMapBounds && otherMapBounds) {
                updateBounds(m, thisMapBounds, otherMapBounds);
            }
        }
    );

    return <div class="maw:mb-4 maw:w-full maw:h-[480px]" ref={mapEl}></div>;
};

export default MapWidget;
