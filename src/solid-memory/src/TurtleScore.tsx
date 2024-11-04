import { Component } from 'solid-js';

import { Player } from './GameStateContext';
import { Style } from './Style';

export type Props = {
    player: Player,
    isFirst: boolean,
    isActive: boolean
};

const TurtleScore: Component<Props> = (props) => {
    const buildClassList = (): {[k: string]: boolean} => {
        const c = {
            "opacity-50": !props.isActive
        }

        c[Style[`turtle-color-${props.player.turtle.color}`]] = props.isActive;
        c[Style[`turtle-border-${props.player.turtle.color}`]] = props.isActive;

        return c;
    };

    return (
        <div class="grow flex justify-around items-center border-b-2 border-b-solid" classList={buildClassList()}>
            <div class="text-16 font-cursive">{props.player.score}</div>
            <div class={`${props.isFirst ? "order-first" : "order-last"}`}><img class="w-20 h-20 rounded-3xl" src={props.player.turtle.imageUrl} /></div>
        </div>
    );
}

export default TurtleScore;
