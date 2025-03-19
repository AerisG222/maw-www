import { Component } from 'solid-js';

import { Player } from './GameStateContext';

export type Props = {
    player: Player,
    isActive: boolean
};

const CharacterScore: Component<Props> = (props) => {
    const buildClassList = (): {[k: string]: boolean} => ({
        "maw:transition": true,
        "maw:opacity-50": !props.isActive,
        "maw:text-[#c00]": props.isActive
    });

    const buildImgClassList = (): {[k:string]: boolean} => ({
        "maw:rounded-xl": true,
        "maw:border-solid": true,
        "maw:border-[transparent]": !props.isActive,
        "maw:border-[#c00]": props.isActive
    });

    return (
        <div class="maw:basis-1/6 maw:flex maw:flex-col maw:items-center maw:justify-center" classList={buildClassList()}>
            <div classList={buildImgClassList()}>
                <img class="maw:w-[150px] maw:h-auto" src={props.player.character.imageUrl} />
            </div>
            <div class="maw:mt-2 maw:text-5xl maw:font-cursive">${props.player.score}</div>
        </div>
    );
}

export default CharacterScore;
