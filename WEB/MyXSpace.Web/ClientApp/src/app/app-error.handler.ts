
import { Injectable, ErrorHandler, Injector } from "@angular/core";
import { AlertService, MessageSeverity } from './services/alert.service';

@Injectable()
export class AppErrorHandler extends ErrorHandler
{
  constructor(private injector: Injector) {
        super();
    }

  private alertService: AlertService;

  handleError(error: any)
  {
        if (this.alertService == null) {
            this.alertService = this.injector.get(AlertService);
        }

        //this.alertService.showStickyMessage("Fatal Error!", "An unresolved error has occured. Please reload the page to correct this error", MessageSeverity.warn);
        //this.alertService.showStickyMessage("Unhandled Error", error.message || error, MessageSeverity.error, error);

        if (confirm("Fatal Error!\nAn unresolved error has occured. Do you want to reload the page to correct this?\n\nError: " + error.message))
            window.location.reload(true);

        super.handleError(error);
    }
}
