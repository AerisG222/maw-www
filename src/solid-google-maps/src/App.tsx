import { createResource, createSignal, Show, Suspense, type Component } from 'solid-js';
import { Loader } from '@googlemaps/js-api-loader';

import MapWidget from './MapWidget';

const App: Component = () => {
    const [address, setAddress] = createSignal("");
    const [poi, setPoi] = createSignal<google.maps.LatLng>();
    const [center, setCenter] = createSignal<google.maps.LatLng | undefined>(
        undefined,
        {
            equals: (prev, next) => prev?.lat == next?.lat && prev?.lng == next?.lng
        }
    );
    const [map1Bounds, setMap1Bounds] = createSignal<google.maps.LatLngBounds | undefined>(
        undefined,
        {
            equals: (prev, next) =>
                prev?.getCenter().lat == next?.getCenter().lat &&
                prev?.getCenter().lng == next?.getCenter().lng
        }
    );
    const [map2Bounds, setMap2Bounds] = createSignal<google.maps.LatLngBounds | undefined>(
        undefined,
        {
            equals: (prev, next) =>
                prev?.getCenter().lat == next?.getCenter().lat &&
                prev?.getCenter().lng == next?.getCenter().lng
        }
    );
    const showMaps = () => center() && poi();

    const loader = new Loader({
        apiKey: "AIzaSyA50h7G5fm_83lh460EnOdabUC9zU8XF7A",
        version: "weekly"
    });

    const initMaps = async () => {
        var libs = await Promise.all([
            loader.importLibrary("maps"),
            loader.importLibrary("geocoding"),
            loader.importLibrary("marker")
        ]);

        return {
            maps: libs[0],
            geocoding: libs[1],
            marker: libs[2]
        }
    };

    const [apis] = createResource(initMaps);

    const showAddress = () => {
        const geocoder = new apis.latest!.geocoding.Geocoder();

        geocoder.geocode({ address: address() }, (results, status) => {
            if (status !== google.maps.GeocoderStatus.OK) {
                alert(`There was an error geocoding the address: ${address()}].  Reported error code = ${status}`);
            } else {
                if(results != null) {
                    setPoi(results[0].geometry.location);
                }

                setCenter(poi);
            }
        });
    };

    const showSampleAddress = (evt: Event, address: string) => {
        evt.preventDefault();
        setAddress(address);
        showAddress();
    };

    return (
        <div class="maw-font-sans">
            <Suspense fallback={<div>Loading...</div>}>
                <p>
                    Specify an address to start at.  You must specify either city and state, postal code, or both.
                </p>
                <p>
                    Example: <a href="#" onClick={(evt) => showSampleAddress(evt, 'The White House')}>The White House</a>
                </p>

                <div class="maw-flex maw-items-center maw-mb-8">
                    <label class="maw-label maw-w-sm">
                        <span class="maw-label-text maw-font-bold maw-mr-4">Address</span>
                        <input type="text" class="maw-input maw-input-bordered maw-w-full maw-max-w-xs maw-color-gray" autofocus value={address()} onInput={e => setAddress(e.target.value)} />
                    </label>

                    <button type="submit" class="maw-ml-8 maw-btn maw-btn-primary" onClick={showAddress}>Submit</button>
                </div>

                <Show when={showMaps()}>
                    <MapWidget
                        mapApi={apis()!.maps}
                        markerApi={apis()!.marker}
                        poi={poi()!}
                        center={center()!}
                        zoom={17}
                        otherMapBounds={map2Bounds()}
                        centerChanged={setCenter}
                        boundsChanged={setMap1Bounds} />

                    <MapWidget
                        mapApi={apis()!.maps}
                        markerApi={apis()!.marker}
                        poi={poi()!}
                        center={center()!}
                        zoom={13}
                        otherMapBounds={map1Bounds()}
                        centerChanged={setCenter}
                        boundsChanged={setMap2Bounds} />
                </Show>
            </Suspense>
        </div>
    );
};

export default App;
