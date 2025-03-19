import { Component } from 'solid-js';

import "./SplashScreen.css";

const SplashScreen: Component = () => {
    return (
        <div class="animate-splash maw:flex maw:justify-center">
            <img class="maw:h-auto maw:max-w-[900px]" src={`${import.meta.env.VITE_ASSET_ROOT}board/splash.png`} />
        </div>
    );
};

export default SplashScreen;
