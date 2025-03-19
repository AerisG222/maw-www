import { Component } from 'solid-js';

import { useGameStateContext } from './GameStateContext';

import TurtleScore from './TurtleScore';

const ScoreBanner: Component = () => {
    const [state] = useGameStateContext();

    return (
        <div class="maw:grid maw:grid-cols-[2fr_1fr_2fr] maw:grid-rows-1 maw:gap-3 maw:mb-4 maw:items-center">
            <TurtleScore
                isFirst={true}
                isActive={state.activePlayer === 1}
                player={state.player1!} />

            <div class="maw:text-center maw:font-cursive maw:text-5xl maw:text-yellow-300">vs</div>

            <TurtleScore
                isFirst={false}
                isActive={state.activePlayer === 2}
                player={state.player2!} />
        </div>
    );
};

export default ScoreBanner;
