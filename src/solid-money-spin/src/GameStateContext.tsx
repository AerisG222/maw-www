import { createContext, ParentComponent, useContext } from "solid-js";
import { createStore } from "solid-js/store";

export type Character = {
    readonly name: string;
    readonly imageUrl: string;
};

export type Player = {
    readonly character: Character;
    readonly score: number;
};

export enum Status {
    Splash,
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
    status: Status.Splash
};

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
    const [state, setState] = createStore(defaultGameState);
    const winningAmount = 25;

    const player1Characters: Character[] = [
        { name: "C3P0",  imageUrl: "players/p1c3p0.png" },
        { name: "R2D2",  imageUrl: "players/p1r2d2.png" },
        { name: "Leo",   imageUrl: "players/p1leo.png" },
        { name: "Mikey", imageUrl: "players/p1mikey.png" }
    ];

    const player2Characters: Character[] = [
        { name: "C3P0",  imageUrl: "players/p2c3p0.png" },
        { name: "R2D2",  imageUrl: "players/p2r2d2.png" },
        { name: "Leo",   imageUrl: "players/p2leo.png" },
        { name: "Mikey", imageUrl: "players/p2mikey.png" }
    ];

    const getPlayer1Characters = () => player1Characters;
    const getPlayer2Characters = () => player2Characters;

    const setPlayerCharacter = (name: string, playerPropName: string, playerCharacters: Character[]): void => {
        const character = playerCharacters.find(x => x.name === name);

        if(!state[playerPropName]) {
            setState(playerPropName, {
                character: character,
                score: 0
            });
        } else {
            setState(playerPropName, {
                character: character
            });
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

    const isGameOver = () => state.player1?.score >= winningAmount || state.player2?.score >= winningAmount;

    const addMoney = (playerPropName: string, amount: number) => {
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

    setTimeout(() => {
        setState("status", Status.PlayerSelect);
    }, 1200);

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

export const useGameStateContext = () => useContext(GameStateContext)!;
