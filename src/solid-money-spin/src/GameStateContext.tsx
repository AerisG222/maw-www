import { createContext, createStore, onSettled, useContext, type ParentComponent } from "solid-js";

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
        finishSpin: (amount: number) => void;
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

        setState(s => {
            const player = s[playerPropName];

            if(player) {
                player.character = character;
            } else {
                s[playerPropName] = { character, score: 0 };
            }
        });
    };

    const setPlayer1Character = (name: string) => {
        setPlayerCharacter(name, "player1", player1Characters);
    };

    const setPlayer2Character = (name: string) => {
        setPlayerCharacter(name, "player2", player2Characters);
    };

    const startGame = () => {
        setState(s => {
            s.activePlayer = 1;
            s.status = Status.Playing;
        });
    };

    // banks the spin for the active player, then either ends the game or passes the turn.
    // one draft, so the game over check sees the new score - reading state after a
    // separate write would still see the old one until the next flush
    const finishSpin = (amount: number) => {
        setState(s => {
            const player = s.activePlayer === 1 ? s.player1! : s.player2!;

            player.score += amount;

            if(player.score >= winningAmount) {
                s.status = Status.GameOver;
            } else {
                s.activePlayer = s.activePlayer === 1 ? 2 : 1;
            }
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

    // the splash screen is time boxed - own the timer through the component lifecycle so
    // it cannot fire after the provider is gone
    onSettled(() => {
        const splashTimeout = setTimeout(() => setState(s => { s.status = Status.PlayerSelect; }), SPLASH_DURATION_MS);

        return () => clearTimeout(splashTimeout);
    });

    return (
        <GameStateContext value={[state, {
            getPlayer1Characters,
            getPlayer2Characters,
            setPlayer1Character,
            setPlayer2Character,
            startGame,
            finishSpin,
            rematch,
            newGame
        }]}>
            {props.children}
        </GameStateContext>
    );
};

// the context has no default, so useContext itself throws when there is no provider
export const useGameStateContext = () => useContext(GameStateContext);
