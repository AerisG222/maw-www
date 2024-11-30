import { createSignal, onCleanup, type Component } from 'solid-js';
import { formatDuration, intervalToDuration, isFriday, isWeekend, nextFriday } from 'date-fns';

const App: Component = () => {
    const WEEKEND_MSG = "Sweet!  It's the weekend!";
    var [countdown, setCountdown] = createSignal("");

    const getCountdownValue = (d: Date) => {
        if(isWeekend(d)) {
            return WEEKEND_MSG;
        }

        var friEow = d;

        if(isFriday(d)) {
            if(d.getHours() >= 17) {
                return WEEKEND_MSG;
            }

            friEow = d;
        } else {
            friEow = nextFriday(d);
        }

        const friAtFive = new Date(friEow.getFullYear(), friEow.getMonth(), friEow.getDate(), 17);
        const duration = intervalToDuration({
            start: d,
            end: friAtFive
        });

        return formatDuration(duration)
            .replace(" days",    "d")
            .replace(" hours",   "h")
            .replace(" minutes", "m")
            .replace(" seconds", "s");
    }

    const interval = setInterval(() => {
        setCountdown(getCountdownValue(new Date()));
    }, 300);

    onCleanup(() => clearInterval(interval));

    return (
        <div class="m-12 font-cursive text-4xl text-center">
            {countdown()}
        </div>
    );
};

export default App;
