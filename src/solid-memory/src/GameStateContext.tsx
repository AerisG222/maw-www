import { createContext, ParentComponent, useContext } from "solid-js";
import { createStore } from "solid-js/store";

export type Turtle = {
    readonly name: string;
    readonly color: string;
    readonly imageUrl: string;
};

export type Player = {
    readonly turtle: Turtle;
    readonly score: number;
};

export enum Status {
    PlayerSelect,
    Playing,
    GameOver
};

export type GameState = {
    readonly player1?: Player;
    readonly player2?: Player;
    readonly activePlayer?: number;
    readonly status: Status;
};

export const defaultGameState: GameState = {
    player1: undefined,
    player2: undefined,
    activePlayer: undefined,
    status: Status.PlayerSelect
};

export type GameStateContextValue = [
    state: GameState,
    actions: {
        getAllTurtles: () => Turtle[];
        setPlayer1Turtle: (name: string) => void;
        setPlayer2Turtle: (name: string) => void;
        startGame: () => void;
        incrementScore: () => void;
        endTurn: () => void;
        endGame: () => void;
        rematch: () => void;
        newGame: () => void;
    }
];

const GameStateContext = createContext<GameStateContextValue>();

export const GameStateProvider: ParentComponent = (props) => {
    const [state, setState] = createStore(defaultGameState);

    const allTurtles: Turtle[] = [
        { name: "Leonardo",      color: "blue",   imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/leonardo.jpg` },
        { name: "Michaelangelo", color: "orange", imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/michaelangelo.jpg` },
        { name: "Donatello",     color: "purple", imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/donatello.jpg` },
        { name: "Raphael",       color: "red",    imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/raphael.jpg` }
    ];

    const getAllTurtles = () => allTurtles;

    const setPlayerTurtle = (name: string, playerPropName: string): void => {
        const turtle = allTurtles.find(x => x.name === name);

        if(!state[playerPropName]) {
            setState(playerPropName, {
                turtle: turtle,
                score: 0
            });
        } else {
            setState(playerPropName, {
                turtle: turtle
            });
        }
    };

    const setPlayer1Turtle = (name: string) => {
        setPlayerTurtle(name, "player1");
    };

    const setPlayer2Turtle = (name: string) => {
        setPlayerTurtle(name, "player2");
    };

    const startGame = () => {
        setState("activePlayer", 1);
        setState("status", Status.Playing);
    }

    const endGame = () => {
        setState("status", Status.GameOver);
    }

    const scorePoint = (playerPropName: string) => {
        setState(playerPropName, "score", score => score + 1);
    };

    const incrementScore = () => {
        state.activePlayer === 1 ? scorePoint("player1") : scorePoint("player2");
    };

    const endTurn = () => {
        setState("activePlayer", activePlayer => activePlayer === 1 ? 2 : 1);
    };

    const rematch = () => {
        setState("status", Status.Playing);
        setState("activePlayer", 1);
        setState("player1", p => ({
            ...p,
            score: 0
        }));
        setState("player2", p => ({
            ...p,
            score: 0
        }));
    };

    const newGame = () => {
        setState("status", Status.PlayerSelect);
        setState("player1", undefined);
        setState("player2", undefined);
    };

    return (
        <GameStateContext.Provider value={[state, {
            getAllTurtles,
            setPlayer1Turtle,
            setPlayer2Turtle,
            startGame,
            incrementScore,
            endTurn,
            endGame,
            rematch,
            newGame
        }]}>
            {props.children}
        </GameStateContext.Provider>
    );
};

export const useGameStateContext = () => useContext(GameStateContext)!;
