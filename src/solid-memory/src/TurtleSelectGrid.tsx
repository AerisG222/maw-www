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
            "cursor-pointer": true,
            "text-center": true,

            "opacity-75": props.selectedTurtle?.name !== turtle.name,
            "opacity-100": props.selectedTurtle?.name === turtle.name,
            "hover:opacity-100": true,

            "border-solid": true,
            "border-[4px]": true,
            "rounded-xl": true,
            "border-color-transparent": props.selectedTurtle?.name !== turtle.name
        };

        classes[Style[`turtle-border-${turtle.color}`]] = props.selectedTurtle?.name === turtle.name;
        classes[Style[`turtle-hover-border-${turtle.color}`]] = true;
        classes[Style[`turtle-color-${turtle.color}`]] = props.selectedTurtle?.name === turtle.name;
        classes[Style[`turtle-hover-color-${turtle.color}`]] = true;

        return classes;
    };

    return (
        <div class="grid grid-rows-[1fr_1fr] grid-cols-[1fr_1fr] gap-4">
        <For each={props.turtles}>{ turtle =>
            <div classList={buildClasses(turtle)} onClick={() => props.selectTurtle(turtle.name)}>
                <img class="rounded-lg h-[192px] w-[150px]" src={turtle.imageUrl} />
                <br/>
                <strong>{turtle.name}</strong>
            </div>
        }</For>
        </div>
    );
};

export default TurtleSelectGrid;
