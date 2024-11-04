import { Component, createEffect, createSignal } from 'solid-js';

import "./Card.css";

export type Props = {
    backUrl: string,
    imageUrl: string,
    turnCardOver: boolean,
    removeCard: boolean,
    enabled: boolean,
    cardSelected: (card: string) => void
};

const Card: Component<Props> = (props) => {
    const [isFlipped, setIsFlipped] = createSignal(false);
    const [isRemoved, setIsRemoved] = createSignal(false);

    const buildClassList = (): {[k: string]: boolean} => ({
        "flip-container": true,
        "center-block": true,
        "cursor-pointer": props.enabled,
        "cursor-not-allowed": !props.enabled,
        "flip": isFlipped(),
        "remove": isRemoved()
    });

    const onClickCard = () => {
        if(!props.enabled) {
            return;
        }

        if(!isFlipped()) {
            setIsFlipped(true);
            props.cardSelected(props.imageUrl);
        }
    };

    createEffect(() => {
        if(props.turnCardOver) {
            setIsFlipped(false);
        }
    });

    createEffect(() => {
        if(props.removeCard) {
            setIsRemoved(true);
        }
    });

    return (
        <div classList={buildClassList()} onClick={onClickCard}>
            <div class="flipper">
                <div class="front">
                    <img src={props.backUrl} />
                </div>
                <div class="back">
                    <img src={props.imageUrl} />
                </div>
            </div>
        </div>
    );
};

export default Card;
