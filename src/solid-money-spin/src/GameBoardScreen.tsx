import { createSignal, type Component } from 'solid-js';
import createRAF, { targetFPS } from '@solid-primitives/raf';

import { useGameStateContext } from './GameStateContext';

import CharacterScore from './CharacterScore';

// where each dollar value ends, walking clockwise around the board
const SECTORS = [
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

const GameBoardScreen: Component = () => {
    const [state, { scoreMoneyValue, endTurn, endGame, isGameOver }] = useGameStateContext();
    const [rotation, setRotation] = createSignal(0);

    // the arrow's velocity is per-frame scratch state, not something the view renders,
    // so it stays a plain local rather than a signal
    let degPerTick = 0;

    const calcScore = () => {
        const deg = 360 - (rotation() % 360);
        const sector = SECTORS.find(s => deg <= s.deg);

        if(!sector) {
            throw new Error(`No sector covers ${deg} degrees!`);
        }

        return sector.dollars;
    };

    const spin = () => {
        setRotation(r => r + degPerTick);

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

    const [running, start, stop] = createRAF(targetFPS(spin, 60));

    const initiateSpin = () => {
        degPerTick = (Math.random() * 30) + 20;
        start();
    };

    return (
        <>
        <div class="maw:flex maw:gap-2">
            <CharacterScore
                isActive={state.activePlayer === 1}
                player={state.player1!} />

            <div class="maw:basis-4/6 maw:grid maw:place-items-center maw:[grid-template-areas:'board']">
                <img class="maw:[grid-area:board]" src={`${import.meta.env.VITE_ASSET_ROOT}board/board.png`} />
                <img class="maw:[grid-area:board]" style={{ transform: `rotate(${rotation()}deg)` }} src={`${import.meta.env.VITE_ASSET_ROOT}board/arrow.png`} />
            </div>

            <CharacterScore
                isActive={state.activePlayer === 2}
                player={state.player2!} />
        </div>
        <div class="maw:my-8 maw:text-center">
            <button class="maw:btn maw:btn-primary maw:px-12" onClick={initiateSpin} disabled={running()}>Spin</button>
        </div>
        </>
    );
};

export default GameBoardScreen;
