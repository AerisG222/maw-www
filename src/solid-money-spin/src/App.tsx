import { Match, Switch, type Component } from 'solid-js';

import { Status, useGameStateContext } from './GameStateContext';

import styles from './App.module.css';
import ChooseCharacterScreen from './ChooseCharacterScreen';
import GameBoardScreen from './GameBoardScreen';
import GameOverScreen from './GameOverScreen';
import SplashScreen from './SplashScreen';

const App: Component = () => {
    const [state] = useGameStateContext();

    // switch = poor mans router
    return (
        <div class={styles.App}>
            <Switch>
                <Match when={state.status === Status.Splash}>
                    <SplashScreen />
                </Match>
                <Match when={state.status === Status.PlayerSelect}>
                    <ChooseCharacterScreen />
                </Match>
                <Match when={state.status === Status.Playing}>
                    <GameBoardScreen />
                </Match>
                <Match when={state.status === Status.GameOver}>
                    <GameOverScreen />
                </Match>
            </Switch>
        </div>
    );
};

export default App;