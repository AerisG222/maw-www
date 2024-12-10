import { Component, Show } from "solid-js";

import { useGameStateContext } from './GameStateContext';

import TurtleSelectGrid from './TurtleSelectGrid';

const ChooseTurtleScreen: Component = () => {
    const [state, { getAllTurtles, setPlayer1Turtle, setPlayer2Turtle, startGame }] = useGameStateContext();

    return (
        <>
            <div class="maw-flex maw-gap-16">
                <h2>Player 1 - Choose Your Turtle</h2>
                <h2>Player 2 - Choose Your Turtle</h2>
            </div>
            <div class="maw-flex maw-gap-16">
                <TurtleSelectGrid
                    turtles={getAllTurtles()}
                    selectedTurtle={state.player1?.turtle}
                    selectTurtle={setPlayer1Turtle} />

                <TurtleSelectGrid
                    turtles={getAllTurtles()}
                    selectedTurtle={state.player2?.turtle}
                    selectTurtle={setPlayer2Turtle} />
            </div>
            <div class="maw-flex maw-gap-16">
                <Show when={state.player1 && state.player2}>
                    <button class="maw-btn maw-btn-primary" onClick={startGame}>Start</button>
                </Show>
            </div>
        </>
    );
};

export default ChooseTurtleScreen;
