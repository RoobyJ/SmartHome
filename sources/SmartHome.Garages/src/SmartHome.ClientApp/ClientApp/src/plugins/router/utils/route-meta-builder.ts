import type { RouteMeta } from 'vue-router';
import { RouteAuthMeta } from './route-auth-meta';

export class RouteMetaBuilder {
    private title: string = '';
    private icon: string | null = null;
    private allowAnonymousFlag = false;
    private onlyAnonymousFlag = false;
    private useDashboardLayoutFlag = true;

    public build(): RouteMeta {
        return {
            title: this.title,
            icon: this.icon,
            authGuard: new RouteAuthMeta(this.allowAnonymousFlag, this.onlyAnonymousFlag, null, null),
            useDashboardLayout: this.useDashboardLayoutFlag,
        };
    }

    public withTitle(title: string): RouteMetaBuilder {
        this.title = title;
        return this;
    }

    public withIcon(icon: string): RouteMetaBuilder {
        this.icon = icon;
        return this;
    }

    public allowAnonymous(): RouteMetaBuilder {
        this.onlyAnonymousFlag = false;
        this.allowAnonymousFlag = true;
        return this;
    }

    public onlyAnonymous(): RouteMetaBuilder {
        this.onlyAnonymousFlag = true;
        this.allowAnonymousFlag = true;
        return this;
    }

    public usePanelLayout(): RouteMetaBuilder {
        this.useDashboardLayoutFlag = false;
        return this;
    }
}
