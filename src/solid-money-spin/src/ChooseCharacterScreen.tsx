import { Component, Show } from "solid-js";

import { useGameStateContext } from './GameStateContext';

import CharacterSelectGrid from './CharacterSelectGrid';

const ChooseCharacterScreen: Component = () => {
    const [state, { getPlayer1Characters, getPlayer2Characters, setPlayer1Character, setPlayer2Character, startGame }] = useGameStateContext();

    const startButtonDisabled = () => !state.player1 || !state.player2;

    return (
        <>
            <div class="maw-flex maw-w-full maw-justify-center">
                <div>
                    <h2 class="maw-text-xl maw-font-bold maw-mb-8">Player 1 - Choose Your Character</h2>

                    <CharacterSelectGrid
                        characters={getPlayer1Characters()}
                        selectedCharacter={state.player1?.character}
                        selectCharacter={setPlayer1Character} />
                </div>
                <div class="maw-divider maw-divider-horizontal" />
                <div>
                    <h2 class="maw-text-xl maw-font-bold maw-mb-8">Player 2 - Choose Your Character</h2>

                    <CharacterSelectGrid
                        characters={getPlayer2Characters()}
                        selectedCharacter={state.player2?.character}
                        selectCharacter={setPlayer2Character} />
                </div>
            </div>

            <div class="maw-flex maw-justify-center maw-my-8">
                <button class="maw-btn maw-btn-primary maw-px-12" onClick={startGame} disabled={startButtonDisabled()}>Start</button>
            </div>
        </>
    );
};

export default ChooseCharacterScreen;
