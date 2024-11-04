import { Component } from 'solid-js';

import { useGameStateContext } from './GameStateContext';

import TurtleScore from './TurtleScore';

const ScoreBanner: Component = () => {
    const [state] = useGameStateContext();

    return (
        <div class="grid grid-cols-[2fr_1fr_2fr] grid-rows-1 gap-3 mb-4 items-center">
            <TurtleScore
                isFirst={true}
                isActive={state.activePlayer === 1}
                player={state.player1!} />

            <div class="text-center font-cursive text-12 c-yellow-3">vs</div>

            <TurtleScore
                isFirst={false}
                isActive={state.activePlayer === 2}
                player={state.player2!} />
        </div>
    );
};

export default ScoreBanner;
