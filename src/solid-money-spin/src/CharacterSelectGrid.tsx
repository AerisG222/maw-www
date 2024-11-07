import { Component, For } from "solid-js";

import { Character } from "./GameStateContext";

export type Props = {
    characters: Character[],
    selectedCharacter?: Character,
    selectCharacter: (character: string) => void
}

const CharacterSelectGrid: Component<Props> = (props) => {
    const buildClasses = (character: Character) => {
        var classes: {[k: string]: boolean} = {
            "cursor-pointer": true,
            "text-center": true,

            "opacity-75": props.selectedCharacter?.name !== character.name,
            "opacity-100": props.selectedCharacter?.name === character.name,
            "hover:opacity-100": true,

            "border-solid": true,
            "border-[4px]": true,
            "rounded-xl": true,
            "border-color-[transparent]": props.selectedCharacter?.name !== character.name,
            "border-color-[red]": props.selectedCharacter?.name === character.name,
            "hover:border-color-[red]": true,
        };

        return classes;
    };

    return (
        <div class="grid grid-rows-[1fr_1fr] grid-cols-[1fr_1fr] gap-4">
        <For each={props.characters}>{ character =>
            <div classList={buildClasses(character)} onClick={() => props.selectCharacter(character.name)}>
                <img class="rounded-lg h-[200px] w-[200px]" src={character.imageUrl} />
            </div>
        }</For>
        </div>
    );
};

export default CharacterSelectGrid;
