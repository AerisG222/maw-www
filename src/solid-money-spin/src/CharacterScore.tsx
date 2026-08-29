import { type Component } from 'solid-js';

import { type Player } from './GameStateContext';

export type Props = {
    player: Player,
    isActive: boolean
};

const CharacterScore: Component<Props> = (props) => {
    return (
        <div
            class="maw:basis-1/6 maw:flex maw:flex-col maw:items-center maw:justify-center maw:transition"
            classList={{
                "maw:opacity-50": !props.isActive,
                "maw:text-[#c00]": props.isActive
            }}
        >
            <div
                class="maw:rounded-xl maw:border-solid"
                classList={{
                    "maw:border-[transparent]": !props.isActive,
                    "maw:border-[#c00]": props.isActive
                }}
            >
                <img class="maw:w-[150px] maw:h-auto" src={props.player.character.imageUrl} />
            </div>
            <div class="maw:mt-2 maw:text-5xl maw:font-cursive">${props.player.score}</div>
        </div>
    );
}

export default CharacterScore;
