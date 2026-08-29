import { createContext, onCleanup, onMount, useContext, type ParentComponent } from "solid-js";
import { createStore } from "solid-js/store";

export type Character = {
    readonly name: string;
    readonly imageUrl: string;
};

export type Player = {
    character: Character;
    score: number;
};

export enum Status {
    Splash,
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

const SPLASH_DURATION_MS = 1200;

// a factory, not a shared object literal - createStore writes through to whatever it
// is handed, so a module level constant would be mutated by the running game
export const createDefaultGameState = (): GameState => ({
    player1: undefined,
    player2: undefined,
    activePlayer: undefined,
    status: Status.Splash
});

export type GameStateContextValue = [
    state: GameState,
    actions: {
        getPlayer1Characters: () => Character[];
        getPlayer2Characters: () => Character[];
        setPlayer1Character: (name: string) => void;
        setPlayer2Character: (name: string) => void;
        startGame: () => void;
        scoreMoneyValue: (amount: number) => void;
        isGameOver: () => boolean;
        endTurn: () => void;
        endGame: () => void;
        rematch: () => void;
        newGame: () => void;
    }
];

const GameStateContext = createContext<GameStateContextValue>();

export const GameStateProvider: ParentComponent = (props) => {
    const [state, setState] = createStore(createDefaultGameState());
    const winningAmount = 25;

    const player1Characters: Character[] = [
        { name: "C3P0",  imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/p1c3p0.png` },
        { name: "R2D2",  imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/p1r2d2.png` },
        { name: "Leo",   imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/p1leo.png` },
        { name: "Mikey", imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/p1mikey.png` }
    ];

    const player2Characters: Character[] = [
        { name: "C3P0",  imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/p2c3p0.png` },
        { name: "R2D2",  imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/p2r2d2.png` },
        { name: "Leo",   imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/p2leo.png` },
        { name: "Mikey", imageUrl: `${import.meta.env.VITE_ASSET_ROOT}players/p2mikey.png` }
    ];

    const getPlayer1Characters = () => player1Characters;
    const getPlayer2Characters = () => player2Characters;

    const setPlayerCharacter = (name: string, playerPropName: PlayerKey, playerCharacters: Character[]): void => {
        const character = playerCharacters.find(x => x.name === name);

        if(!character) {
            return;
        }

        if(state[playerPropName]) {
            setState(playerPropName, "character", character);
        } else {
            setState(playerPropName, { character, score: 0 });
        }
    };

    const setPlayer1Character = (name: string) => {
        setPlayerCharacter(name, "player1", player1Characters);
    };

    const setPlayer2Character = (name: string) => {
        setPlayerCharacter(name, "player2", player2Characters);
    };

    const startGame = () => {
        setState("activePlayer", 1);
        setState("status", Status.Playing);
    };

    const endGame = () => {
        setState("status", Status.GameOver);
    };

    const isGameOver = () => (state.player1?.score ?? 0) >= winningAmount || (state.player2?.score ?? 0) >= winningAmount;

    const addMoney = (playerPropName: PlayerKey, amount: number) => {
        setState(playerPropName, "score", currentValue => currentValue + amount);
    };

    const scoreMoneyValue = (amount: number) => {
        state.activePlayer === 1 ? addMoney("player1", amount) : addMoney("player2", amount);
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

    // the splash screen is time boxed - own the timer through the component lifecycle so
    // it cannot fire after the provider is gone
    onMount(() => {
        const splashTimeout = setTimeout(() => setState("status", Status.PlayerSelect), SPLASH_DURATION_MS);

        onCleanup(() => clearTimeout(splashTimeout));
    });

    return (
        <GameStateContext.Provider value={[state, {
            getPlayer1Characters,
            getPlayer2Characters,
            setPlayer1Character,
            setPlayer2Character,
            startGame,
            scoreMoneyValue,
            isGameOver,
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
