import { type Component } from 'solid-js';

import { type Player } from './GameStateContext';

export type Props = {
    player: Player,
    isFirst: boolean,
    isActive: boolean
};

const TurtleScore: Component<Props> = (props) => {
    return (
        <div
            class="maw:grow maw:flex maw:justify-around maw:items-center maw:border-b-2 maw:border-b-solid maw:transition"
            classList={{
                "maw:opacity-50": !props.isActive,
                [props.player.turtle.styles.textColorClass]: props.isActive,
                [props.player.turtle.styles.borderColorClass]: props.isActive
            }}
        >
            <div class="maw:text-6xl maw:font-cursive">{props.player.score}</div>
            <div classList={{ "maw:order-first": props.isFirst, "maw:order-last": !props.isFirst }}>
                <img class="maw:size-20 maw:rounded-3xl" src={props.player.turtle.imageUrl} />
            </div>
        </div>
    );
};

export default TurtleScore;
