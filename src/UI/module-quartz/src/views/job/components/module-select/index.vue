<script>
import { mixins } from 'netmodular-ui'

const api = $api.quartz.job

export default {
  mixins: [mixins.select],
  data() {
    return {
      action: api.moduleSelect
    }
  }
}
</script>
