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
        <div class="maw-grow maw-flex maw-justify-around maw-items-center maw-border-b-2 maw-border-b-solid" classList={buildClassList()}>
            <div class="maw-text-16 maw-font-cursive">{props.player.score}</div>
            <div class={`${props.isFirst ? "maw-order-first" : "maw-order-last"}`}><img class="maw-w-20 maw-h-20 maw-rounded-3xl" src={props.player.turtle.imageUrl} /></div>
        </div>
    );
}

export default TurtleScore;
