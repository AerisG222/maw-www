import { Component, For } from "solid-js";

import { Turtle } from "./GameStateContext";
import { Style } from './Style';

export type Props = {
    turtles: Turtle[],
    selectedTurtle?: Turtle,
    selectTurtle: (turtle: string) => void
}

const TurtleSelectGrid: Component<Props> = (props) => {
    const buildClasses = (turtle: Turtle) => {
        var classes: {[k: string]: boolean} = {
            "maw-cursor-pointer": true,
            "maw-text-center": true,

            "maw-opacity-75": props.selectedTurtle?.name !== turtle.name,
            "maw-opacity-100": props.selectedTurtle?.name === turtle.name,
            "hover:maw-opacity-100": true,

            "maw-border-solid": true,
            "maw-border-[4px]": true,
            "maw-rounded-xl": true,
            "maw-border-color-transparent": props.selectedTurtle?.name !== turtle.name
        };

        classes[Style[`turtle-border-${turtle.color}`]] = props.selectedTurtle?.name === turtle.name;
        classes[Style[`turtle-hover-border-${turtle.color}`]] = true;
        classes[Style[`turtle-color-${turtle.color}`]] = props.selectedTurtle?.name === turtle.name;
        classes[Style[`turtle-hover-color-${turtle.color}`]] = true;

        return classes;
    };

    return (
        <div class="maw-grid maw-grid-rows-[1fr_1fr] maw-grid-cols-[1fr_1fr] maw-gap-4">
        <For each={props.turtles}>{ turtle =>
            <div classList={buildClasses(turtle)} onClick={() => props.selectTurtle(turtle.name)}>
                <img class="maw-rounded-lg maw-h-auto maw-w-full maw-max-w-[150px]" src={turtle.imageUrl} />
                <br/>
                <strong>{turtle.name}</strong>
            </div>
        }</For>
        </div>
    );
};

export default TurtleSelectGrid;
