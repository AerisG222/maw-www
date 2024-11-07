import { Component } from 'solid-js';

import { useGameStateContext } from './GameStateContext';

import "./GameOverScreen.css";

const GameOverScreen: Component = () => {
    const [state, { rematch, newGame }] = useGameStateContext();

    const winner = () => state.player1!.score > state.player2!.score ? state.player1 : state.player2;

    return (
        <div class="min-w-[640px]">
            <div class="animate-winner">
                <h3 class="text-center text-16 font-cursive color-[#c00]">Winner!</h3>

                <div class="mb-8 text-center">
                    <img class="h-[200px] w-[200px]" src={winner()?.character.imageUrl} />
                </div>

                <div class="my-8 text-center">
                    <button class="mr-4" onClick={rematch}>Rematch</button>
                    <button class="mr-4" onClick={newGame}>New Game</button>
                </div>
            </div>
        </div>
    );
};

export default GameOverScreen;
