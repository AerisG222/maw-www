/* @refresh reload */
import { render } from 'solid-js/web';

import './index.css';
import App from './App';
import './google-maps-loader.js';

const root = document.getElementById('root');

if (import.meta.env.DEV && !(root instanceof HTMLElement)) {
    throw new Error(
        'Root element not found. Did you forget to add it to your index.html? Or maybe the id attribute got misspelled?',
    );
}

Promise.all([
    google.maps.importLibrary("core"),
    google.maps.importLibrary("maps"),
    google.maps.importLibrary("geocoding"),
    google.maps.importLibrary("marker")
]);

render(() => <App />, root! );
