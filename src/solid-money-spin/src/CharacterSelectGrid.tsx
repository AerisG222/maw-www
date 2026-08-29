import { For, type Component } from "solid-js";

import { type Character } from "./GameStateContext";

export type Props = {
    characters: Character[],
    selectedCharacter?: Character,
    selectCharacter: (character: string) => void
}

const CharacterSelectGrid: Component<Props> = (props) => {
    const isSelected = (character: Character) => props.selectedCharacter?.name === character.name;

    return (
        <div class="maw:grid maw:grid-rows-[1fr_1fr] maw:grid-cols-[1fr_1fr] maw:gap-4">
        <For each={props.characters}>{ character =>
            <div
                class="maw:transition maw:cursor-pointer maw:text-center maw:hover:opacity-100 maw:border-solid maw:border-[4px] maw:rounded-xl maw:hover:border-[red]"
                classList={{
                    "maw:opacity-75": !isSelected(character),
                    "maw:opacity-100": isSelected(character),
                    "maw:border-[transparent]": !isSelected(character),
                    "maw:border-[red]": isSelected(character)
                }}
                onClick={() => props.selectCharacter(character.name)}
            >
                <img class="maw:rounded-lg maw:h-auto maw:max-w-[140px] 2xl:maw:max-w-[200px]" src={character.imageUrl} />
            </div>
        }</For>
        </div>
    );
};

export default CharacterSelectGrid;
