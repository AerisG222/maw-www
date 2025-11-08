import { createSignal, Show, Suspense, type Component } from 'solid-js';

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

    const showAddress = () => {
        const geocoder = new google.maps.Geocoder();

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
        <div class="maw:font-sans">
            <Suspense fallback={<div>Loading...</div>}>
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
            </Suspense>
        </div>
    );
};

export default App;
