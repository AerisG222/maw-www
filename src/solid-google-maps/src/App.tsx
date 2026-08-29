import { createResource, createSignal, Show, Suspense, type Component } from 'solid-js';

import MapWidget from './MapWidget';

// the maps api is injected by google-maps-loader.js, but the individual libraries
// load lazily.  resolving them through a resource lets Suspense show the fallback
// until the api is genuinely usable.
const loadMapsApi = async () => {
    await Promise.all([
        google.maps.importLibrary("core"),
        google.maps.importLibrary("maps"),
        google.maps.importLibrary("geocoding"),
        google.maps.importLibrary("marker")
    ]);

    return true;
};

// the maps types carry their own value equality, so hand it to the signals rather
// than reaching into the objects ourselves
const sameLatLng = (prev?: google.maps.LatLng, next?: google.maps.LatLng) =>
    prev === next || (!!prev && !!next && prev.equals(next));

const sameBounds = (prev?: google.maps.LatLngBounds, next?: google.maps.LatLngBounds) =>
    prev === next || (!!prev && !!next && prev.equals(next));

const App: Component = () => {
    const [mapsApi] = createResource(loadMapsApi);
    const [address, setAddress] = createSignal("");
    const [poi, setPoi] = createSignal<google.maps.LatLng>();
    const [center, setCenter] = createSignal<google.maps.LatLng | undefined>(undefined, { equals: sameLatLng });
    const [map1Bounds, setMap1Bounds] = createSignal<google.maps.LatLngBounds | undefined>(undefined, { equals: sameBounds });
    const [map2Bounds, setMap2Bounds] = createSignal<google.maps.LatLngBounds | undefined>(undefined, { equals: sameBounds });

    const showMaps = () => !!center() && !!poi();

    const showAddress = () => {
        const geocoder = new google.maps.Geocoder();

        geocoder.geocode({ address: address() }, (results, status) => {
            if (status !== google.maps.GeocoderStatus.OK || !results?.length) {
                alert(`There was an error geocoding the address: ${address()}].  Reported error code = ${status}`);

                return;
            }

            const location = results[0].geometry.location;

            setPoi(location);
            setCenter(location);
        });
    };

    const showSampleAddress = (evt: Event, sampleAddress: string) => {
        evt.preventDefault();
        setAddress(sampleAddress);
        showAddress();
    };

    return (
        <div class="maw:font-sans">
            <Suspense fallback={<div>Loading...</div>}>
                <Show when={mapsApi()}>
                    <p>
                        Specify an address to start at.  You must specify either city and state, postal code, or both.
                    </p>
                    <p>
                        Example: <a href="#" class="maw:text-secondary" onClick={(evt) => showSampleAddress(evt, 'The White House')}>The White House</a>
                    </p>

                    <div class="not-prose">
                        <div class="maw:flex maw:items-center maw:my-4">
                            <fieldset class="maw:fieldset">
                                <label class="maw:input maw:w-xl">
                                    <span class="maw:label maw:font-bold maw:text-secondary">Address</span>
                                    <input type="text" autofocus value={address()} onInput={e => setAddress(e.target.value)} />
                                </label>
                            </fieldset>

                            <button type="submit" class="maw:ml-8 maw:btn maw:btn-primary" onClick={showAddress}>Submit</button>
                        </div>

                        <Show when={showMaps()}>
                            <MapWidget
                                poi={poi()!}
                                center={center()!}
                                zoom={17}
                                otherMapBounds={map2Bounds()}
                                centerChanged={setCenter}
                                boundsChanged={setMap1Bounds} />

                            <MapWidget
                                poi={poi()!}
                                center={center()!}
                                zoom={13}
                                otherMapBounds={map1Bounds()}
                                centerChanged={setCenter}
                                boundsChanged={setMap2Bounds} />
                        </Show>
                    </div>
                </Show>
            </Suspense>
        </div>
    );
};

export default App;
