import { createSignal, For, onCleanup, type Component } from 'solid-js';
import { createStore } from 'solid-js/store';

import { useGameStateContext } from './GameStateContext';

import Card from './Card';
import ScoreBanner from './ScoreBanner';

type BoardCard = {
    id: number;
    imageUrl: string;
    flipped: boolean;
    removed: boolean;
};

const BACK_CARD = `${import.meta.env.VITE_ASSET_ROOT}cards/back.jpg`;
const ALL_CARDS = [
    `${import.meta.env.VITE_ASSET_ROOT}cards/card1.jpg`,
    `${import.meta.env.VITE_ASSET_ROOT}cards/card2.jpg`,
    //`${import.meta.env.VITE_ASSET_ROOT}cards/card3.jpg`,  // exclude to use 10 for game
    `${import.meta.env.VITE_ASSET_ROOT}cards/card4.jpg`,
    `${import.meta.env.VITE_ASSET_ROOT}cards/card5.jpg`,
    `${import.meta.env.VITE_ASSET_ROOT}cards/card6.jpg`,
    `${import.meta.env.VITE_ASSET_ROOT}cards/card7.jpg`,
    `${import.meta.env.VITE_ASSET_ROOT}cards/card8.jpg`,
    `${import.meta.env.VITE_ASSET_ROOT}cards/card9.jpg`,
    `${import.meta.env.VITE_ASSET_ROOT}cards/card10.jpg`,
    `${import.meta.env.VITE_ASSET_ROOT}cards/card11.jpg`
];

// from MDN:
// Returns a random integer between min (included) and max (excluded)
// Using Math.round() will give you a non-uniform distribution!
const getRandomInt = (min: number, max: number): number => {
    return Math.floor(Math.random() * (max - min)) + min;
};

// http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
const shuffle = (cards: string[]): void => {
    for (let i = 0; i < cards.length; i++) {
        const rand = getRandomInt(i, cards.length);
        const tmp = cards[i];
        cards[i] = cards[rand];
        cards[rand] = tmp;
    }
};

const generateGameBoard = (): BoardCard[] => {
    const gameCards = [...ALL_CARDS, ...ALL_CARDS];

    for(let i = 0; i < 3; i++) {
        shuffle(gameCards);
    }

    return gameCards.map((imageUrl, id) => ({ id, imageUrl, flipped: false, removed: false }));
};

const GameBoardScreen: Component = () => {
    const [state, { incrementScore, endTurn, endGame }] = useGameStateContext();
    const [board, setBoard] = createStore(generateGameBoard());
    const [selected, setSelected] = createSignal<number[]>([]);
    const [evaluatingTurn, setEvaluatingTurn] = createSignal(false);

    // the turn animations are driven off timers, so make sure none of them outlive the screen
    const pendingTimeouts = new Set<ReturnType<typeof setTimeout>>();

    const schedule = (fn: () => void, delay: number) => {
        const id = setTimeout(() => {
            pendingTimeouts.delete(id);
            fn();
        }, delay);

        pendingTimeouts.add(id);
    };

    onCleanup(() => {
        pendingTimeouts.forEach(clearTimeout);
        pendingTimeouts.clear();
    });

    const isGameOver = () => state.player1!.score + state.player2!.score === ALL_CARDS.length;

    const onMatch = (picks: number[]) => {
        incrementScore();

        schedule(() => setBoard(picks, "removed", true), 500);

        schedule(() => {
            setSelected([]);
            setEvaluatingTurn(false);

            if(isGameOver()) {
                endGame();
            }
        }, 1300);
    };

    const onMiss = (picks: number[]) => {
        schedule(() => setBoard(picks, "flipped", false), 700);

        schedule(() => {
            setSelected([]);
            endTurn();
            setEvaluatingTurn(false);
        }, 800);
    };

    const evaluateTurn = (picks: number[]) => {
        setEvaluatingTurn(true);

        if(board[picks[0]].imageUrl === board[picks[1]].imageUrl) {
            onMatch(picks);
        } else {
            onMiss(picks);
        }
    };

    const onCardSelected = (id: number) => {
        if(evaluatingTurn() || board[id].flipped) {
            return;
        }

        setBoard(id, "flipped", true);

        const picks = [...selected(), id];

        setSelected(picks);

        if(picks.length === 2) {
            evaluateTurn(picks);
        }
    };

    return (
        <div>
            <ScoreBanner />

            <div class="maw:grid
                maw:grid-cols-[repeat(5,1fr)]
                maw:grid-rows-[repeat(4,1fr)]
                maw:gap-4 maw:place-items-center maw:mx-auto maw:w-min
            ">
            <For each={board}>{ card =>
                <Card
                    backUrl={BACK_CARD}
                    imageUrl={card.imageUrl}
                    enabled={!evaluatingTurn()}
                    flipped={card.flipped}
                    removed={card.removed}
                    cardSelected={() => onCardSelected(card.id)} />
            }</For>
            </div>
        </div>
    );
};

export default GameBoardScreen;
