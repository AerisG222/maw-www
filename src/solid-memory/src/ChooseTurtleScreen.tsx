import { Component, Show } from "solid-js";

import { useGameStateContext } from './GameStateContext';

import TurtleSelectGrid from './TurtleSelectGrid';

const ChooseTurtleScreen: Component = () => {
    const [state, { getAllTurtles, setPlayer1Turtle, setPlayer2Turtle, startGame }] = useGameStateContext();

    return (
        <>
            <div class="maw:flex maw:w-full maw:justify-center">
                <div>
                    <h2 class="maw:text-xl maw:font-bold maw:mb-8">Player 1 - Choose Your Turtle</h2>

                    <TurtleSelectGrid
                        turtles={getAllTurtles()}
                        selectedTurtle={state.player1?.turtle}
                        selectTurtle={setPlayer1Turtle} />
                </div>
                <div class="maw:divider maw:divider-horizontal" />
                <div>
                    <h2 class="maw:text-xl maw:font-bold maw:mb-8">Player 2 - Choose Your Turtle</h2>

                    <TurtleSelectGrid
                        turtles={getAllTurtles()}
                        selectedTurtle={state.player2?.turtle}
                        selectTurtle={setPlayer2Turtle} />
                </div>
            </div>

            <div class="maw:flex maw:justify-center maw:mt-8">
                <Show when={state.player1 && state.player2}>
                    <button class="maw:btn maw:btn-primary" onClick={startGame}>Start</button>
                </Show>
            </div>
        </>
    );
};

export default ChooseTurtleScreen;
