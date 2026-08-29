import { For, type Component } from "solid-js";

import { type Turtle } from "./GameStateContext";

export type Props = {
    turtles: Turtle[],
    selectedTurtle?: Turtle,
    selectTurtle: (turtle: string) => void
}

const TurtleSelectGrid: Component<Props> = (props) => {
    const isSelected = (turtle: Turtle) => props.selectedTurtle?.name === turtle.name;

    return (
        <div class="maw:grid maw:grid-rows-[1fr_1fr] maw:grid-cols-[1fr_1fr] maw:gap-4">
        <For each={props.turtles}>{ turtle =>
            <div
                class={`maw:transition maw:cursor-pointer maw:text-center maw:hover:opacity-100 maw:border-solid maw:border-[4px] maw:rounded-xl ${turtle.styles.borderHoverColorClass} ${turtle.styles.textHoverColorClass}`}
                classList={{
                    "maw:opacity-100": isSelected(turtle),
                    "maw:opacity-75": !isSelected(turtle),
                    "maw:border-transparent": !isSelected(turtle),
                    [turtle.styles.borderColorClass]: isSelected(turtle),
                    [turtle.styles.textColorClass]: isSelected(turtle)
                }}
                onClick={() => props.selectTurtle(turtle.name)}
            >
                <img class="maw:rounded-lg maw:h-auto maw:w-full maw:max-w-[150px] 2xl:maw:max-w-[200px]" src={turtle.imageUrl} />
                <br/>
                <strong>{turtle.name}</strong>
            </div>
        }</For>
        </div>
    );
};

export default TurtleSelectGrid;
