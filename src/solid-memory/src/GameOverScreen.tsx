import { Show, type Component } from 'solid-js';

import { useGameStateContext } from './GameStateContext';

import "./GameOverScreen.css";
import ScoreBanner from './ScoreBanner';

const GameOverScreen: Component = () => {
    const [state, { rematch, newGame }] = useGameStateContext();

    const isTie = () => state.player1!.score === state.player2!.score;
    const winner = () => state.player1!.score > state.player2!.score ? state.player1 : state.player2;

    return (
        <div class="maw:min-w-[640px]">
            <ScoreBanner />
            <div class="animate-winner">
                <Show when={!isTie() && winner()} fallback={
                    <>
                        <div class="maw:flex maw:justify-center maw:mt-16 maw:mb-8 maw:gap-24">
                            <img class="maw:h-[192px] maw:w-[150px]" src={state.player1?.turtle.imageUrl} />
                            <img class="maw:h-[192px] maw:w-[150px]" src={state.player2?.turtle.imageUrl} />
                        </div>
                        <div class="maw:font-cursive maw:text-6xl maw:c-yellow-3 maw:text-center maw:mt-4">Tie!</div>
                    </>
                }>{ theWinner =>
                    <>
                        <div class="maw:mt-16 maw:mb-8 maw:text-center">
                            <img class="maw:inline maw:h-[192px] maw:w-[150px]" src={theWinner().turtle.imageUrl} />
                        </div>
                        <div class={`maw:font-cursive maw:text-6xl maw:my-4 maw:text-center ${theWinner().turtle.styles.textColorClass}`}>{theWinner().turtle.name} Wins!</div>
                        <div class="maw:font-cursive maw:text-4xl maw:c-yellow-3 maw:text-center">Flawless Victory!</div>
                    </>
                }</Show>

                <div class="maw:my-8 maw:text-center">
                    <button class="maw:btn maw:btn-primary maw:mr-4" onClick={rematch}>Rematch</button>
                    <button class="maw:btn maw:btn-primary maw:mr-4" onClick={newGame}>New Game</button>
                </div>
            </div>
        </div>
    );
};

export default GameOverScreen;
