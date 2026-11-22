import { createMemo, createSignal, Loading, Show, type Component } from 'solid-js';

import MapWidget from './MapWidget';
import { createSiteTheme } from './siteTheme';

// the maps api is injected by google-maps-loader.js, but the individual libraries
// load lazily.  resolving them through an async memo lets Loading show the fallback
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
    const mapsApi = createMemo(loadMapsApi);
    const [address, setAddress] = createSignal("");
    const [poi, setPoi] = createSignal<google.maps.LatLng>();
    const [center, setCenter] = createSignal<google.maps.LatLng | undefined>(undefined, { equals: sameLatLng });
    const [map1Bounds, setMap1Bounds] = createSignal<google.maps.LatLngBounds | undefined>(undefined, { equals: sameBounds });
    const [map2Bounds, setMap2Bounds] = createSignal<google.maps.LatLngBounds | undefined>(undefined, { equals: sameBounds });
    // held here rather than in the widgets so a zoom outlives the map it was made on
    const [map1Zoom, setMap1Zoom] = createSignal(17);
    const [map2Zoom, setMap2Zoom] = createSignal(13);
    const siteTheme = createSiteTheme();

    const showMaps = () => !!center() && !!poi();

    // takes the address rather than reading address(): after setAddress, the signal still
    // reads the old value until the next flush
    const showAddress = (addressToShow: string) => {
        const geocoder = new google.maps.Geocoder();

        geocoder.geocode({ address: addressToShow }, (results, status) => {
            if (status !== google.maps.GeocoderStatus.OK || !results?.length) {
                alert(`There was an error geocoding the address: ${addressToShow}].  Reported error code = ${status}`);

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
        showAddress(sampleAddress);
    };

    return (
        <div class="maw:font-sans">
            <Loading fallback={<div>Loading...</div>}>
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

                            <button type="submit" class="maw:ml-8 maw:btn maw:btn-primary" onClick={() => showAddress(address())}>Submit</button>
                        </div>

                        <Show when={showMaps()}>
                            {/* a map takes its color scheme only when made, so keying on the theme
                                swaps in new maps when it changes - center, zoom and the poi all live
                                up here, so the new maps pick up where the old ones were */}
                            <Show when={siteTheme()} keyed>
                                {theme => {
                                    const colorScheme = theme === 'dark' ? google.maps.ColorScheme.DARK : google.maps.ColorScheme.LIGHT;

                                    return (
                                        <>
                                            <MapWidget
                                                poi={poi()!}
                                                center={center()!}
                                                zoom={map1Zoom()}
                                                colorScheme={colorScheme}
                                                otherMapBounds={map2Bounds()}
                                                centerChanged={setCenter}
                                                boundsChanged={setMap1Bounds}
                                                zoomChanged={setMap1Zoom} />

                                            <MapWidget
                                                poi={poi()!}
                                                center={center()!}
                                                zoom={map2Zoom()}
                                                colorScheme={colorScheme}
                                                otherMapBounds={map1Bounds()}
                                                centerChanged={setCenter}
                                                boundsChanged={setMap2Bounds}
                                                zoomChanged={setMap2Zoom} />
                                        </>
                                    );
                                }}
                            </Show>
                        </Show>
                    </div>
                </Show>
            </Loading>
        </div>
    );
};

export default App;
