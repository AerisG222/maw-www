import { Component, Show } from "solid-js";

import { useGameStateContext } from './GameStateContext';

import CharacterSelectGrid from './CharacterSelectGrid';

const ChooseCharacterScreen: Component = () => {
    const [state, { getPlayer1Characters, getPlayer2Characters, setPlayer1Character, setPlayer2Character, startGame }] = useGameStateContext();

    return (
        <>
            <div class="flex gap-16">
                <h2>Player 1 - Choose Your Character</h2>
                <h2>Player 2 - Choose Your Character</h2>
            </div>
            <div class="flex gap-16">
                <CharacterSelectGrid
                    characters={getPlayer1Characters()}
                    selectedCharacter={state.player1?.character}
                    selectCharacter={setPlayer1Character} />

                <CharacterSelectGrid
                    characters={getPlayer2Characters()}
                    selectedCharacter={state.player2?.character}
                    selectCharacter={setPlayer2Character} />
            </div>
            <div class="flex gap-16">
                <Show when={state.player1 && state.player2}>
                    <button onClick={startGame}>Start</button>
                </Show>
            </div>
        </>
    );
};

export default ChooseCharacterScreen;
