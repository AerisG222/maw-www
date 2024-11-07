import { Component, createSignal } from 'solid-js';

import { useGameStateContext } from './GameStateContext';

import CharacterScore from './CharacterScore';
import createRAF, { targetFPS } from '@solid-primitives/raf';

const GameBoardScreen: Component = () => {
    const [state, { scoreMoneyValue, endTurn, endGame, isGameOver }] = useGameStateContext();
    const [rotation, setRotation] = createSignal(0);
    var degPerTick = 0;

    const sectors = [
        { deg: 37, dollars: 8 },
        { deg: 97, dollars: 6 },
        { deg: 128, dollars: 4 },
        { deg: 157, dollars: 1 },
        { deg: 178, dollars: 9 },
        { deg: 219, dollars: 7 },
        { deg: 265, dollars: 5 },
        { deg: 286, dollars: 3 },
        { deg: 313, dollars: 2 },
        { deg: 360, dollars: 10 }
    ];

    const calcScore = () => {
        const deg = 360 - (rotation() % 360);

        for(var sector of sectors) {
            if(deg <= sector.deg) {
                return sector.dollars;
            }
        }

        throw new Error("Should not get here!");
    };

    const spin = () => {
        setRotation(rotation() + degPerTick);

        degPerTick = degPerTick - (Math.random() * 0.4);

        if(degPerTick <= 0) {
            stop();

            scoreMoneyValue(calcScore());

            if(isGameOver()) {
                endGame();
            } else {
                endTurn();
            }
        }
    };

    const [running, start, stop] = createRAF(
        targetFPS(timeStamp => spin(), 60)
    );

    const initiateSpin = () => {
        degPerTick = (Math.random() * 30) + 20;
        start();
    };

    return (
        <>
        <div class="grid grid-cols-[min-content_auto_min-content] gap-4">
            <CharacterScore
                isActive={state.activePlayer === 1}
                player={state.player1!} />

            <div class="relative">
                <img class="relative" src="board/board.png" />
                <img class="absolute left-[200px] top-[149px]" style={{ transform: `rotate(${rotation()}deg)` }} src="board/arrow.png" />
            </div>

            <CharacterScore
                isActive={state.activePlayer === 2}
                player={state.player2!} />
        </div>
        <div>
            <button onClick={() => initiateSpin()} disabled={running()}>Spin</button>
        </div>
        </>
    );
};

export default GameBoardScreen;
