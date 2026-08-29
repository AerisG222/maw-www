import { createContext, useContext, type ParentComponent } from "solid-js";
import { createStore } from "solid-js/store";

export type Turtle = {
    readonly name: string;
    readonly imageUrl: string;
    readonly styles: {
        readonly borderColorClass: string;
        readonly borderHoverColorClass: string;
        readonly textColorClass: string;
        readonly textHoverColorClass: string;
    }
};

export type Player = {
    turtle: Turtle;
    score: number;
};

export enum Status {
    PlayerSelect,
    Playing,
    GameOver
};

export type PlayerKey = "player1" | "player2";

export type GameState = {
    player1?: Player;
    player2?: Player;
    activePlayer?: number;
    status: Status;
};

// a factory, not a shared object literal - createStore writes through to whatever it
// is handed, so a module level constant would be mutated by the running game
export const createDefaultGameState = (): GameState => ({
    player1: undefined,
    player2: undefined,
    activePlayer: undefined,
    status: Status.PlayerSelect
});

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
    const [state, setState] = createStore(createDefaultGameState());

    const allTurtles: Turtle[] = [
        {
            name: "Leonardo",
            imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/leonardo.jpg`,
            styles: {
                borderColorClass: "maw:border-leonardo",
                borderHoverColorClass: "maw:hover:border-leonardo",
                textColorClass: "maw:text-leonardo",
                textHoverColorClass: "maw:hover:text-leonardo"
            }
        },
        {
            name: "Michaelangelo",
            imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/michaelangelo.jpg`,
            styles: {
                borderColorClass: "maw:border-michaelangelo",
                borderHoverColorClass: "maw:hover:border-michaelangelo",
                textColorClass: "maw:text-michaelangelo",
                textHoverColorClass: "maw:hover:text-michaelangelo"
            }
        },
        {
            name: "Donatello",
            imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/donatello.jpg`,
            styles: {
                borderColorClass: "maw:border-donatello",
                borderHoverColorClass: "maw:hover:border-donatello",
                textColorClass: "maw:text-donatello",
                textHoverColorClass: "maw:hover:text-donatello"
            }
        },
        {
            name: "Raphael",
            imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/raphael.jpg`,
            styles: {
                borderColorClass: "maw:border-raphael",
                borderHoverColorClass: "maw:hover:border-raphael",
                textColorClass: "maw:text-raphael",
                textHoverColorClass: "maw:hover:text-raphael"
            }
        }
    ];

    const getAllTurtles = () => allTurtles;

    const setPlayerTurtle = (name: string, playerPropName: PlayerKey): void => {
        const turtle = allTurtles.find(x => x.name === name);

        if(!turtle) {
            return;
        }

        if(state[playerPropName]) {
            setState(playerPropName, "turtle", turtle);
        } else {
            setState(playerPropName, { turtle, score: 0 });
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

    const scorePoint = (playerPropName: PlayerKey) => {
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
        setState("player1", "score", 0);
        setState("player2", "score", 0);
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

export const useGameStateContext = () => {
    const context = useContext(GameStateContext);

    if(!context) {
        throw new Error("useGameStateContext must be used inside a GameStateProvider");
    }

    return context;
};
