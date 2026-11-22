import { createContext, createStore, useContext, type ParentComponent } from "solid-js";

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

        setState(s => {
            const player = s[playerPropName];

            if(player) {
                player.turtle = turtle;
            } else {
                s[playerPropName] = { turtle, score: 0 };
            }
        });
    };

    const setPlayer1Turtle = (name: string) => {
        setPlayerTurtle(name, "player1");
    };

    const setPlayer2Turtle = (name: string) => {
        setPlayerTurtle(name, "player2");
    };

    const startGame = () => {
        setState(s => {
            s.activePlayer = 1;
            s.status = Status.Playing;
        });
    }

    const endGame = () => {
        setState(s => {
            s.status = Status.GameOver;
        });
    }

    const incrementScore = () => {
        setState(s => {
            const player = s.activePlayer === 1 ? s.player1 : s.player2;

            player!.score += 1;
        });
    };

    const endTurn = () => {
        setState(s => {
            s.activePlayer = s.activePlayer === 1 ? 2 : 1;
        });
    };

    const rematch = () => {
        setState(s => {
            s.status = Status.Playing;
            s.activePlayer = 1;
            s.player1!.score = 0;
            s.player2!.score = 0;
        });
    };

    const newGame = () => {
        setState(s => {
            s.status = Status.PlayerSelect;
            s.player1 = undefined;
            s.player2 = undefined;
        });
    };

    return (
        <GameStateContext value={[state, {
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
        </GameStateContext>
    );
};

// the context has no default, so useContext itself throws when there is no provider
export const useGameStateContext = () => useContext(GameStateContext);
