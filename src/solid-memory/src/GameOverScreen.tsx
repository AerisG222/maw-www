import { Component, Show } from 'solid-js';

import { useGameStateContext } from './GameStateContext';
import { Style } from './Style';

import "./GameOverScreen.css";
import ScoreBanner from './ScoreBanner';

const GameOverScreen: Component = () => {
    const [state, { rematch, newGame }] = useGameStateContext();

    const isTie = () => state.player1!.score === state.player2!.score;
    const winner = () => state.player1!.score > state.player2!.score ? state.player1 : state.player2;

    return (
        <div class="min-w-[640px]">
            <ScoreBanner />
            <div class="animate-winner">
                <Show when={isTie()}>
                    <div class="flex justify-center mb-8 gap-24">
                        <img class="h-[192px] w-[150px]" src={state.player1?.turtle.imageUrl} />
                        <img class="h-[192px] w-[150px]" src={state.player2?.turtle.imageUrl} />
                    </div>
                    <div class="font-cursive text-12 c-yellow-3 text-center mt-4">Tie!</div>
                </Show>
                <Show when={!isTie()}>
                    <div class="mb-8 text-center">
                        <img class="h-[192px] w-[150px]" src={winner()?.turtle.imageUrl} />
                    </div>
                    <div class={`font-cursive text-12 my-4 text-center ${Style[`turtle-color-${winner()!.turtle.color}`]}`}>{winner()?.turtle.name} Wins!</div>
                    <div class="font-cursive text-12 c-yellow-3 text-center">Flawless Victory!</div>
                </Show>

                <div class="my-8 text-center">
                    <button class="mr-4" onClick={rematch}>Rematch</button>
                    <button class="mr-4" onClick={newGame}>New Game</button>
                </div>
            </div>
        </div>
    );
};

export default GameOverScreen;
