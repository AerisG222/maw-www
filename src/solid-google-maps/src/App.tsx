import { createResource, createSignal, Show, Suspense, type Component } from 'solid-js';
import { Loader } from '@googlemaps/js-api-loader';

import styles from './App.module.css';
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
        <div class={styles.App}>
            <Suspense fallback={<div>Loading...</div>}>
                <p>
                    Specify an address to start at.  You must specify either city and state, or the postal code, or both.
                    <br />
                    Example: <a href="#" onClick={(evt) => showSampleAddress(evt, 'The White House')}>The White House</a>
                </p>

                <h2>Address:</h2>
                <div class={styles.mb3}>
                    <input type="text" value={address()} onChange={(evt) => setAddress(evt.target.value)} />
                </div>
                <div class={styles.mb3}>
                    <button type="submit" onClick={showAddress} class="btn btn-primary">Submit</button>
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
