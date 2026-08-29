import { type Component } from 'solid-js';

import "./Card.css";

export type Props = {
    backUrl: string,
    imageUrl: string,
    flipped: boolean,
    removed: boolean,
    enabled: boolean,
    cardSelected: () => void
};

const Card: Component<Props> = (props) => {
    const onClickCard = () => {
        if(!props.enabled || props.flipped) {
            return;
        }

        props.cardSelected();
    };

    return (
        <div
            classList={{
                "flip-container": true,
                "center-block": true,
                "cursor-pointer": props.enabled,
                "cursor-not-allowed": !props.enabled,
                "flip": props.flipped,
                "remove": props.removed
            }}
            onClick={onClickCard}
        >
            <div class="flipper maw:w-[100px] md:maw:w-[120px] maw:h-[100px] md:maw:h-[120px]">
                <div class="front maw:w-[100px] md:maw:w-[120px] maw:h-[100px] md:maw:h-[120px]">
                    <img class="maw:w-[100px] md:maw:w-[120px] maw:h-[100px] md:maw:h-[120px]" src={props.backUrl} />
                </div>
                <div class="back maw:w-[100px] md:maw:w-[120px] maw:h-[100px] md:maw:h-[120px]">
                    <img class="maw:w-[100px] md:maw:w-[120px] maw:h-[100px] md:maw:h-[120px]" src={props.imageUrl} />
                </div>
            </div>
        </div>
    );
};

export default Card;
