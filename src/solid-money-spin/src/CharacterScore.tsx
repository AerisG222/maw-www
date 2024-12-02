import { Component } from 'solid-js';

import { Player } from './GameStateContext';

export type Props = {
    player: Player,
    isActive: boolean
};

const CharacterScore: Component<Props> = (props) => {
    const buildClassList = (): {[k: string]: boolean} => ({
        "maw-opacity-50": !props.isActive,
        "maw-color-[#c00]": props.isActive
    });

    const buildImgClassList = (): {[k:string]: boolean} => ({
        "maw-rounded-xl": true,
        "maw-border-solid": true,
        "maw-border-color-[transparent]": !props.isActive,
        "maw-border-color-[#c00]": props.isActive
    });

    return (
        <div class="maw-basis-1/6 maw-flex maw-flex-col maw-items-center maw-justify-center" classList={buildClassList()}>
            <div classList={buildImgClassList()}>
                <img class="maw-w-[150px] maw-h-auto" src={props.player.character.imageUrl} />
            </div>
            <div class="maw-text-12 maw-font-cursive">${props.player.score}</div>
        </div>
    );
}

export default CharacterScore;
