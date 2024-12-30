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
            "maw-transition": true,
            "maw-cursor-pointer": true,
            "maw-text-center": true,

            "maw-opacity-75": props.selectedCharacter?.name !== character.name,
            "maw-opacity-100": props.selectedCharacter?.name === character.name,
            "hover:maw-opacity-100": true,

            "maw-border-solid": true,
            "maw-border-[4px]": true,
            "maw-rounded-xl": true,
            "maw-border-color-[transparent]": props.selectedCharacter?.name !== character.name,
            "maw-border-color-[red]": props.selectedCharacter?.name === character.name,
            "hover:maw-border-color-[red]": true,
        };

        return classes;
    };

    return (
        <div class="maw-grid maw-grid-rows-[1fr_1fr] maw-grid-cols-[1fr_1fr] maw-gap-4">
        <For each={props.characters}>{ character =>
            <div classList={buildClasses(character)} onClick={() => props.selectCharacter(character.name)}>
                <img class="maw-rounded-lg maw-h-auto maw-max-w-[140px] 2xl:maw-max-w-[200px]" src={character.imageUrl} />
            </div>
        }</For>
        </div>
    );
};

export default CharacterSelectGrid;
