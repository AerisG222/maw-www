import { Component } from 'solid-js';

import { Player } from './GameStateContext';

export type Props = {
    player: Player,
    isFirst: boolean,
    isActive: boolean
};

const TurtleScore: Component<Props> = (props) => {
    const buildClassList = () => {
        const classes = [
            "maw:grow",
            "maw:flex",
            "maw:justify-around",
            "maw:items-center",
            "maw:border-b-2",
            "maw:border-b-solid",
            "maw:transition",
        ];

        if(props.isActive) {
            classes.push(props.player.turtle.styles.textColorClass);
            classes.push(props.player.turtle.styles.borderColorClass);
        } else {
            classes.push("maw:opacity-50");
        }

        return classes.join(" ");
    };

    return (
        <div class={buildClassList()}>
            <div class="maw:text-6xl maw:font-cursive">{props.player.score}</div>
            <div class={`${props.isFirst ? "maw:order-first" : "maw:order-last"}`}><img class="maw:size-20 maw:rounded-3xl" src={props.player.turtle.imageUrl} /></div>
        </div>
    );
}

export default TurtleScore;
