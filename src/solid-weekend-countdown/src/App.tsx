import { createSignal, onCleanup, type Component } from 'solid-js';
import { formatDuration, intervalToDuration, isFriday, isWeekend, nextFriday } from 'date-fns';

const WEEKEND_MSG = "Sweet!  It's the weekend!";

const getCountdownValue = (d: Date) => {
    if(isWeekend(d)) {
        return WEEKEND_MSG;
    }

    let friEow = d;

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
};

const App: Component = () => {
    const [now, setNow] = createSignal(new Date());
    const countdown = () => getCountdownValue(now());

    const interval = setInterval(() => setNow(new Date()), 300);

    onCleanup(() => clearInterval(interval));

    return (
        <div class="maw:m-12 maw:font-cursive maw:text-6xl maw:text-center">
            {countdown()}
        </div>
    );
};

export default App;
