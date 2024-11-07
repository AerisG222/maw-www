import { Component } from 'solid-js';

import { Player } from './GameStateContext';

export type Props = {
    player: Player,
    isActive: boolean
};

const CharacterScore: Component<Props> = (props) => {
    const buildClassList = (): {[k: string]: boolean} => ({
        "opacity-50": !props.isActive,
        "color-[#c00]": props.isActive
    });

    const buildImgClassList = (): {[k:string]: boolean} => ({
        "rounded-xl": true,
        "border-solid": true,
        "border-color-[transparent]": !props.isActive,
        "border-color-[#c00]": props.isActive
    });

    return (
        <div class="flex flex-col items-center justify-center" classList={buildClassList()}>
            <div classList={buildImgClassList()}>
                <img class="w-[150px] h-[150px]" src={props.player.character.imageUrl} />
            </div>
            <div class="text-16 font-cursive">${props.player.score}</div>
        </div>
    );
}

export default CharacterScore;
