import { Component, createSignal, For } from 'solid-js';

import { useGameStateContext } from './GameStateContext';

import Card from './Card';
import ScoreBanner from './ScoreBanner';

const GameBoardScreen: Component = () => {
    const [state, { incrementScore, endTurn, endGame }] = useGameStateContext();
    const [selectedCard1, setSelectedCard1] = createSignal<string|undefined>();
    const [selectedCard2, setSelectedCard2] = createSignal<string|undefined>();
    const [turnCardOver, setTurnCardOver] = createSignal(false);
    const [removeCards, setRemoveCards] = createSignal(false);
    const [evaluatingTurn, setEvaluatingTurn] = createSignal(false);
    const backCard = "cards/back.jpg";
    const allCards = [
        "cards/card1.jpg",
        "cards/card2.jpg",
        //"cards/card3.jpg",  // exclude to use 10 for game
        "cards/card4.jpg",
        "cards/card5.jpg",
        "cards/card6.jpg",
        "cards/card7.jpg",
        "cards/card8.jpg",
        "cards/card9.jpg",
        "cards/card10.jpg",
        "cards/card11.jpg"
    ];

    const isGameOver = () => state.player1!.score + state.player2!.score === allCards.length;

    const clearSelectedCards = () => {
        setSelectedCard1(undefined);
        setSelectedCard2(undefined);
    };

    const onMatch = () => {
        incrementScore();

        setTimeout(() => {
            setRemoveCards(true);
            setRemoveCards(false);
        }, 500);

        setTimeout(() => {
            if(isGameOver()) {
                endGame();
            }

            clearSelectedCards();
            setEvaluatingTurn(false);
        }, 1300);
    };

    const onMiss = () => {
        setTimeout(() => {
            // true ensures cards are face down, false prepares for next turn
            setTurnCardOver(true);
            setTurnCardOver(false);
        }, 700);

        setTimeout(() => {
            clearSelectedCards();
            endTurn();
            setEvaluatingTurn(false);
        }, 800);
    };

    const evaluateTurn = () => {
        setEvaluatingTurn(true);

        if(selectedCard1() === selectedCard2()) {
            onMatch();
        } else {
            onMiss();
        }
    };

    const onCardSelected = (card: string) => {
        if(evaluatingTurn()) {
            return;
        }

        if(selectedCard1()) {
            setSelectedCard2(card);
            evaluateTurn();
        } else {
            setSelectedCard1(card);
        }
    };

    // from MDN:
    // Returns a random integer between min (included) and max (excluded)
    // Using Math.round() will give you a non-uniform distribution!
    const getRandomInt = (min: number, max: number): number => {
        return Math.floor(Math.random() * (max - min)) + min;
    }

    // http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    const shuffle = (cards: string[]): void => {
        for (let i = 0; i < cards.length; i++) {
            const rand = getRandomInt(i, cards.length);
            const tmp = cards[i];
            cards[i] = cards[rand];
            cards[rand] = tmp;
        }
    }

    const generateGameBoard = (): string[] => {
        const gameCards = [...allCards, ...allCards];

        for(var i = 0; i < 3; i++) {
            shuffle(gameCards);
        }

        return gameCards;
    }

    var board = generateGameBoard();

    const shouldTurnOver = (card: string) => turnCardOver() && (card === selectedCard1() || card === selectedCard2());
    const shouldRemove = (card: string) => removeCards() && (card === selectedCard1() || card === selectedCard2());

    return (
        <div>
            <ScoreBanner />

            <div class="grid grid-cols-[repeat(5,1fr)] grid-rows-[repeat(4,1fr)] gap-4">
            <For each={board}>{ card =>
                <Card
                    backUrl={backCard}
                    imageUrl={card}
                    enabled={!evaluatingTurn()}
                    turnCardOver={shouldTurnOver(card)}
                    removeCard={shouldRemove(card)}
                    cardSelected={onCardSelected} />
            }</For>
            </div>
        </div>
    );
};

export default GameBoardScreen;