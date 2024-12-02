import { Component } from 'solid-js';

import { useGameStateContext } from './GameStateContext';

import "./GameOverScreen.css";

const GameOverScreen: Component = () => {
    const [state, { rematch, newGame }] = useGameStateContext();

    const winner = () => state.player1!.score > state.player2!.score ? state.player1 : state.player2;

    return (
        <div class="animate-winner maw-flex maw-flex-col maw-justify-center">
            <h3 class="maw-text-center maw-text-16 maw-font-cursive maw-color-[#c00]">Winner!</h3>

            <div class="maw-flex maw-justify-center maw-mt-8">
                <img class="maw-h-auto maw-w-[200px]" src={winner()?.character.imageUrl} />
            </div>

            <div class="maw-my-8 maw-text-center">
                <button class="maw-btn maw-btn-primary maw-mr-4 maw-px-8" onClick={newGame}>New Game</button>
                <button class="maw-btn maw-btn-secondary maw-mr-4 maw-px-8" onClick={rematch}>Rematch</button>
            </div>
        </div>
    );
};

export default GameOverScreen;
