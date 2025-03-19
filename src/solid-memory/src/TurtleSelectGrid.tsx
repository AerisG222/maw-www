import { Component, For } from "solid-js";

import { Turtle } from "./GameStateContext";

export type Props = {
    turtles: Turtle[],
    selectedTurtle?: Turtle,
    selectTurtle: (turtle: string) => void
}

const TurtleSelectGrid: Component<Props> = (props) => {
    const buildClasses = (turtle: Turtle) => {
        var classes = [
            "maw:transition",
            "maw:cursor-pointer",
            "maw:text-center",
            "maw:hover:opacity-100",
            "maw:border-solid",
            "maw:border-[4px]",
            "maw:rounded-xl",
            turtle.styles.borderHoverColorClass,
            turtle.styles.textHoverColorClass,
        ];

        if(props.selectedTurtle?.name === turtle.name) {
            classes.push("maw:opacity-100");
            classes.push(turtle.styles.borderColorClass);
            classes.push(turtle.styles.textColorClass);
        } else {
            classes.push("maw:opacity-75");
            classes.push("maw:border-transparent");
        }

        return classes.join(" ");
    };

    return (
        <div class="maw:grid maw:grid-rows-[1fr_1fr] maw:grid-cols-[1fr_1fr] maw:gap-4">
        <For each={props.turtles}>{ turtle =>
            <div class={buildClasses(turtle)} onClick={() => props.selectTurtle(turtle.name)}>
                <img class="maw:rounded-lg maw:h-auto maw:w-full maw:max-w-[150px] 2xl:maw:max-w-[200px]" src={turtle.imageUrl} />
                <br/>
                <strong>{turtle.name}</strong>
            </div>
        }</For>
        </div>
    );
};

export default TurtleSelectGrid;
