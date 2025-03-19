import { Match, Switch, type Component } from 'solid-js';

import { Status, useGameStateContext } from './GameStateContext';

import ChooseTurtleScreen from './ChooseTurtleScreen';
import GameBoardScreen from './GameBoardScreen';
import GameOverScreen from './GameOverScreen';

const App: Component = () => {
    const [state] = useGameStateContext();

    // switch = poor mans router
    return (
        <div class="maw:flex maw:flex-col maw:place-content-center maw:m-4">
            <Switch>
                <Match when={state.status === Status.PlayerSelect}>
                    <ChooseTurtleScreen />
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
